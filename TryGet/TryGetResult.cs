using System;

namespace TryGet
{
    public class TryGetResult
    {
        public bool IsSuccess { get; }
        public object Value { get; }

        public TryGetResult(bool isSuccess, object value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

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

        public T OrDefault() => IsSuccess ? Value : default;

        public static implicit operator T(TryGetResult<T> value) => value.Value;
    }
}
