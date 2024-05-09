namespace DomainDrivers.SmartSchedule.Planning.Parallelization;

public class StageParallelization
{
    public ParallelStagesList Of(ISet<Stage> stages)
    {
        var parallelStagesList = new ParallelStagesList(new List<ParallelStages>());
        var stagesWithoutDependencies = new HashSet<Stage>(stages.Where(x => !x.Dependencies.Any())
            .ToHashSet());
        
        stages.ExceptWith(stagesWithoutDependencies);

        parallelStagesList.All.Add(new ParallelStages(stagesWithoutDependencies.ToHashSet()));
        
        while (stagesWithoutDependencies.Any())
        {
            var node = stagesWithoutDependencies.First();
            stagesWithoutDependencies.Remove(node);
            var parallelStages = new ParallelStages(new HashSet<Stage>());
            
            foreach (var dependency in stages.Where(x => x.Dependencies.Contains(node)).ToList())
            {
                stages.Remove(dependency);

                if (stages.All(x => x != dependency))
                {
                    stagesWithoutDependencies.Add(dependency);
                    parallelStages.Stages.Add(dependency);
                }
            }

            if (parallelStages.Stages.Any())
            {
                parallelStagesList.All.Add(parallelStages);
            }
        }

        return stages.Any() ? new ParallelStagesList(new List<ParallelStages>()) : parallelStagesList;
    }
}