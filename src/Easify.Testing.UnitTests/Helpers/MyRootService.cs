﻿using System;

 namespace Easify.Testing.UnitTests.Helpers
{
    public class MyRootService
    {
        private readonly IMyService _myService;

        public MyRootService(IMyService myService)
        {
            _myService = myService ?? throw new ArgumentNullException(nameof(myService));
        }

        public void DoWork()
        {
            _myService.DoWork();
        }

        public void DoWork(string value)
        {
            _myService.DoWork(value);
        }
    }
}