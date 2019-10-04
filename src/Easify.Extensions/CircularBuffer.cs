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

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Easify.Extensions
{
    public class CircularBuffer<T> : ICircularBuffer<T>
    {
        private readonly ConcurrentQueue<T> _buffer = new ConcurrentQueue<T>();
        private readonly int _bufferSize;
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        public CircularBuffer(int bufferSize)
        {
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            _bufferSize = bufferSize;
        }

        public IEnumerator<T> GetEnumerator()
        {
            try
            {
                _cacheLock.EnterReadLock();
                return _buffer.GetEnumerator();
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T value)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                if (_buffer.Count == _bufferSize)
                {
                    T valueToDiscard;
                    _buffer.TryDequeue(out valueToDiscard);
                }

                _buffer.Enqueue(value);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }
    }
}