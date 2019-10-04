// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


// Copyright (c) 2014 Logentries

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using Serilog.Debugging;

namespace Serilog.Sinks.Logentries.Sinks.Logentries
{
    internal class LeClient
    {
        // Logentries API server address. 
        private const string LeApiUrl = "api.logentries.com";

        // Port number for token logging on Logentries API server. 
        private const int LeApiTokenPort = 10000;

        // Port number for TLS encrypted token logging on Logentries API server 
        private const int LeApiTokenTlsPort = 20000;

        // Port number for HTTP PUT logging on Logentries API server. 
        private const int LeApiHttpPort = 80;

        // Port number for SSL HTTP PUT logging on Logentries API server. 
        private const int LeApiHttpsPort = 443;
        private TcpClient m_Client;
        private SslStream m_SslStream;
        private Stream m_Stream;
        private readonly int m_TcpPort;

        private readonly bool m_UseSsl;

        public LeClient(bool useHttpPut, bool useSsl)
        {
            m_UseSsl = useSsl;
            if (!m_UseSsl)
                m_TcpPort = useHttpPut ? LeApiHttpPort : LeApiTokenPort;
            else
                m_TcpPort = useHttpPut ? LeApiHttpsPort : LeApiTokenTlsPort;
        }

        private Stream ActiveStream => m_UseSsl ? m_SslStream : m_Stream;

        public void Connect()
        {
            m_Client = new TcpClient
            {
                NoDelay = true
            };
            m_Client.ConnectAsync(LeApiUrl, m_TcpPort).Wait();

            m_Stream = m_Client.GetStream();

            if (m_UseSsl)
            {
                m_SslStream = new SslStream(m_Stream);
                m_SslStream.AuthenticateAsClientAsync(LeApiUrl).Wait();
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            ActiveStream.Write(buffer, offset, count);
        }

        public void Flush()
        {
            ActiveStream.Flush();
        }

        public void Close()
        {
            if (m_Client != null)
                try
                {
                    m_Client.Dispose();
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine("Exception while closing client: {0}", ex);
                }
        }
    }
}