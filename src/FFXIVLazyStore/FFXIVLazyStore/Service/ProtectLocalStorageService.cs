using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLazyStore.Service
{
    public interface IProtectLocalStorageService
    {
        Task<ProtectedResult<T>> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
    }

    public class ProtectLocalStorageService : IProtectLocalStorageService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public ProtectLocalStorageService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<ProtectedResult<T>> GetAsync<T>(string key)
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<T>(key);
                return new ProtectedResult<T>(success: result.Success, value: result.Value);
            }
            catch
            {
                return new ProtectedResult<T>(success: false, value: default);
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }

            await _protectedLocalStorage.SetAsync(key, value);
        }
    }

    public class ProtectedResult<T>
    {
        public ProtectedResult(bool success, T? value)
        {
            Success = success;
            Value = value;
        }

        public bool Success { get; }
        public T? Value { get; }
    }
}
