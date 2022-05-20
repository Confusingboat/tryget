using System;

namespace TryGet
{
    public class TryGetResult
    {
        private readonly object _value;

        public bool IsSuccess { get; }

        public TryGetResult(bool isSuccess, object value)
        {
            IsSuccess = isSuccess;
            _value = value;
        }

        public object Value => IsSuccess ? _value : throw new NoValueException();

        public TryGetResult<T> As<T>()
        {
            var isSuccess = IsSuccess && Value is T;
            return new TryGetResult<T>(isSuccess, isSuccess ? (T)Value : default);
        }

        public bool As<T>(out T value)
        {
            var result = As<T>();
            value = result.Value;
            return result.IsSuccess;
        }

        public static implicit operator bool(TryGetResult result) => result.IsSuccess;
    }

    public class TryGetResult<T> : TryGetResult
    {
        private readonly T _value;

        public TryGetResult(bool isSuccess, T value) : base(isSuccess, value)
        {
            _value = value;
        }

        public new T Value => IsSuccess ? _value : throw new NoValueException();

        public T OrDefault(T value = default) => IsSuccess ? Value : value;

        public static implicit operator T(TryGetResult<T> value) => value.Value;
    }
}
