using DryIoc;
using Microsoft.Extensions.DependencyInjection;

namespace Eft.Launcher;

public static class Extensions1
{
	internal static void _E000(this IContainer container, ServiceDescriptor descriptor)
	{
		if (descriptor.ImplementationType != null)
		{
			IReuse reuse = ((descriptor.Lifetime == ServiceLifetime.Singleton) ? Reuse.Singleton : ((descriptor.Lifetime == ServiceLifetime.Scoped) ? Reuse.ScopedOrSingleton : Reuse.Transient));
			container.Register(descriptor.ServiceType, descriptor.ImplementationType, reuse);
		}
		else if (descriptor.ImplementationFactory != null)
		{
			IReuse reuse2 = ((descriptor.Lifetime == ServiceLifetime.Singleton) ? Reuse.Singleton : ((descriptor.Lifetime == ServiceLifetime.Scoped) ? Reuse.ScopedOrSingleton : Reuse.Transient));
			container.RegisterDelegate(isChecked: true, descriptor.ServiceType, descriptor.ImplementationFactory, reuse2);
		}
		else
		{
			container.RegisterInstance(isChecked: true, descriptor.ServiceType, descriptor.ImplementationInstance);
		}
	}
}
