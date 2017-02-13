using Ozzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ozzy.Server
{
    public interface IExtensibleOptions
    {
        IEnumerable<IOptionsExtension> Extensions { get; }
        TExtension FindExtension<TExtension>() where TExtension : class, IOptionsExtension, new();
        Type OptionsType { get; }
    }

    public interface IExtensibleOptions<out T> : IExtensibleOptions
    {
        IExtensibleOptions<T> UpdateOption<TExtension>(Action<TExtension> updateAction) where TExtension : class, IOptionsExtension, new();
    }

    public abstract class ExtensibleOptions : IExtensibleOptions
    {
        private readonly IReadOnlyDictionary<Type, IOptionsExtension> _extensions;

        protected ExtensibleOptions(IReadOnlyDictionary<Type, IOptionsExtension> extensions)
        {
            Guard.ArgumentNotNull(extensions, nameof(extensions));
            _extensions = extensions;
        }

        public virtual IEnumerable<IOptionsExtension> Extensions => _extensions.Values;

        public virtual TExtension FindExtension<TExtension>()
            where TExtension : class, IOptionsExtension, new()
        {
            return _extensions.TryGetValue(typeof(TExtension), out var extension) ? (TExtension)extension : null;
        }

        public virtual TExtension GetExtension<TExtension>()
            where TExtension : class, IOptionsExtension, new()
        {
            var extension = FindExtension<TExtension>();
            if (extension == null)
            {
                throw new InvalidOperationException($"Options Extension Not Found {typeof(TExtension).FullName}");
            }
            return extension;
        }

        public abstract ExtensibleOptions WithExtension<TExtension>(TExtension extension)
            where TExtension : class, IOptionsExtension, new();

        public abstract Type OptionsType { get; }
    }

    public class ExtensibleOptions<T> : ExtensibleOptions, IExtensibleOptions<T>
    {
        public ExtensibleOptions()
            : base(new Dictionary<Type, IOptionsExtension>())
        {
        }

        public ExtensibleOptions(IReadOnlyDictionary<Type, IOptionsExtension> extensions)
            : base(extensions)
        {
        }

        private ExtensibleOptions<T> WithExtensionInternal<TExtension>(TExtension extension)
             where TExtension : class, IOptionsExtension, new()
        {
            Guard.ArgumentNotNull(extension, nameof(extension));

            var extensions = Extensions.ToDictionary(p => p.GetType(), p => p);
            extensions[typeof(TExtension)] = extension;

            return new ExtensibleOptions<T>(extensions);
        }

        public override ExtensibleOptions WithExtension<TExtension>(TExtension extension)
        {
            return WithExtensionInternal(extension);
        }

        public override Type OptionsType => typeof(T);

        public IExtensibleOptions<T> UpdateOption<TExtension>(Action<TExtension> updateAction)
             where TExtension : class, IOptionsExtension, new()
        {
            var extension = FindExtension<TExtension>() ?? new TExtension();
            updateAction(extension);
            return WithExtensionInternal(extension);
        }
    }

}
