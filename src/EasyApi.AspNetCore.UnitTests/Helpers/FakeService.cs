// This software is part of the EasyApi framework
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

﻿using System.Threading.Tasks;

namespace EasyApi.AspNetCore.UnitTests.Helpers
{
    public sealed class FakeService : IFakeService
    {
        private const int TaskDelayInMs = 1000;

        public string State { get; private set; } = string.Empty;

        public async Task<TArg> CallAndReturnResultAsync<TArg>(TArg input)
        {
            await Task.Delay(TaskDelayInMs);
            return await Task.Run(async () =>
            {
                State = "Changed";
                await Task.Delay(TaskDelayInMs);
                return input;
            });
        }

        public TArg CallAndReturnResult<TArg>(TArg input)
        {
            State = "Changed";
            return input;
        }

        public void Call()
        {
            State = "Changed";
        }

        public async Task CallAsync()
        {
            await Task.Delay(TaskDelayInMs).ContinueWith(t =>
            {
                State = "Changed";
            });
        }

        public async Task CallAndThrowAsync()
        {
            await Task.Delay(TaskDelayInMs);
            await Task.Run(() =>
            {
                State = "Changed";
                throw new FakeException();
            });
        }

        public void CallAndThrow()
        {
            State = "Changed";
            throw new FakeException();
        }

        public async Task<TArg> CallAndThrowInsteadOfReturnResultAsync<TArg>(TArg input)
        {
            await Task.Delay(TaskDelayInMs);
            await Task.Run(async () =>
            {
                State = "Changed";
                await Task.Delay(TaskDelayInMs);
                await Task.Run(() => throw new FakeException());
            });

            return input;
        }
    }
}