using NaughtyAttributes;
using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : ShowIfAttributeBase
    {
        public ShowIfAttribute(string condition)
            : base(condition)
        {
            Inverted = false;
        }

        public ShowIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = false;
        }

        public ShowIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = false;
        }
    }
}


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class IGPAttribute : ShowIfAttributeBase
{
	public IGPAttribute(string condition = "ShowGPVariables")
		: base(condition)
	{
		Inverted = false;
	}

	public IGPAttribute(EConditionOperator conditionOperator, params string[] conditions)
		: base(conditionOperator, conditions)
	{
		Inverted = false;
	}

	public IGPAttribute(string enumName, object enumValue)
		: base(enumName, enumValue as Enum)
	{
		Inverted = false;
	}
}

public class SMAttribute : ShowIfAttributeBase
{
	public SMAttribute(string condition = "shouldMove")
		: base(condition)
	{
		Inverted = false;
	}

	public SMAttribute(EConditionOperator conditionOperator, params string[] conditions)
		: base(conditionOperator, conditions)
	{
		Inverted = false;
	}

	public SMAttribute(string enumName, object enumValue)
		: base(enumName, enumValue as Enum)
	{
		Inverted = false;
	}
}
public class SWCAttribute : ShowIfAttributeBase
{
	public SWCAttribute(string condition = "shouldSwitchColors")
		: base(condition)
	{
		Inverted = false;
	}

	public SWCAttribute(EConditionOperator conditionOperator, params string[] conditions)
		: base(conditionOperator, conditions)
	{
		Inverted = false;
	}

	public SWCAttribute(string enumName, object enumValue)
		: base(enumName, enumValue as Enum)
	{
		Inverted = false;
	}
}