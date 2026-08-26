using System;
using Unity.GraphToolkit.Editor;

[Serializable]
public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context) 
    { 
        context.AddOutputPort("out").Build();
    }
}

[Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
    }
}

[Serializable]
public class DialogueNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddOutputPort("out").Build();
        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Text").Build();
    }
}
[Serializable]public class DecisionNode:Node
{
    const string optionID = "portCount";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddInputPort<string>("Speaker").Build();
        context.AddInputPort<string>("Text").Build();
        INodeOption option = GetNodeOptionByName(optionID);
        option.TryGetValue(out int portCount);

        for (int i = 0; i < portCount; i++) 
        { 
            context.AddInputPort<string>("Decision Text"+i).Build();
            context.AddOutputPort("Decision" + i).Build();
        }
    }
    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<int>(optionID).WithDefaultValue(2).Delayed().Build();
    }
}