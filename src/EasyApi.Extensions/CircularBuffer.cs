using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace EasyApi.Extensions
{
    public class CircularBuffer<T> : ICircularBuffer<T>
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly int _bufferSize;
        private readonly ConcurrentQueue<T> _buffer = new ConcurrentQueue<T>();

        public CircularBuffer(int bufferSize)
        {
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
            this._bufferSize = bufferSize;
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
    }
}
