using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Units.Fighting;

public class UnitsArenaHostedService : IHostedService, IDisposable
{
    private readonly IUnitsService _unitsService;
    private readonly IClock _clock;

    private Task? CombatTask { get; set; }

    public UnitsArenaHostedService(IUnitsService unitsService, IClock clock)
    {
        _unitsService = unitsService;
        _clock = clock;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        CombatTask = StartCombatTask();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task StartCombatTask()
    {
        var fightingUnits = _unitsService.GetFightingUnits();

        while (CombatTask?.IsCanceled != true)
        {
            foreach (var unit in fightingUnits)
            {
                if (_clock.Now.Second % unit.AttackPeriod != 0) continue;

                var otherFightingUnits = fightingUnits.Where(u => unit.User.Id != u.User.Id).ToList();
                unit.Combat(otherFightingUnits);
            }

            var deadUnits = fightingUnits.Where(u => u.Health <= 0).ToList();
            foreach (var deadUnit in deadUnits)
            {
                _unitsService.RemoveUnit(deadUnit.User, deadUnit);
            }

            await _clock.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public void Dispose()
    {
        if (CombatTask != null && (CombatTask.IsCompleted || CombatTask.IsFaulted || CombatTask.IsCanceled))
        {
            CombatTask.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }
}