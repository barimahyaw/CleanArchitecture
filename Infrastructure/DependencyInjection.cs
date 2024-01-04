using Infrastructure.BackgroundJobs;
using Infrastructure.Idempotence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    triggerBuilder =>
                        triggerBuilder.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));

            //[Obsolete]
           // configure.UseMicrosoftDependencyInjectionJobFactory();

        });

        services.AddQuartzHostedService();

        return services;
    }

    public static IServiceCollection AddIdempotentConfig(this IServiceCollection services)
    {
        // the decorator pattern is used to make sure that the domain event handlers are idempotent
        // come from the Scrutor library
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        return services;
    }
}
