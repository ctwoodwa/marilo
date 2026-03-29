namespace Marilo.Core.Enums;

public enum ChangeType
{
    Created,
    Updated,
    Deleted,
    Toggled
}

public enum FilterOperator
{
    Equals,
    NotEquals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    IsNull,
    IsNotNull
}

public enum SortDirection
{
    Ascending,
    Descending
}
