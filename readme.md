QueryForDapper.Postgres
=======================

Purpose
-------

The purpose of this library library is to build syntactically correct parameterized Postgres SQL statements for injection into dapper queries. 

# Configuration

QueryForDapper is designed for one time configuration.

Access Configuration:
```csharp
Query.ConfigureTo()
```


### Naming
A naming scheme is required to be configured prior to use. The library includes the following default naming schemes.

```csharp
Query.ConfigureTo().UseDefaultNaming();
Query.ConfigureTo().UsePassthroughNaming();
Query.ConfigureTo().UseSnakeCaseNaming();
```
| Type | Name | PassthroughNaming | DefaultNaming | SnakeCaseNaming |
|------|------|-------------------|---------------|-----------------|
|Class      | DemoType | DemoType   | DemoTypes | demo_types |
|Property   | DemoProp  | DemoProp  | DemoProp  | demo_prop  |


#### Custom Naming

You can define custom naming delegate by passing a `Func<string, string>` to `.NameColumnsWith()` and `.NameTablesWith()`. 
Alternative, you can implement `INamingScheme` and pass the implementing type to `.UseCustomNamingScheme()`

#### Attribute Naming

Additionally, you can configure QueryForDapper to utilize `ColumnAttribute` and `TableAttribute`.

```csharp
Query.ConfigureTo().UseColumnAttributeNames()
                   .UseTableAttributeNames();
```

#### Naming Defintions

The last naming option is defining a column name or table name during configuration.

```csharp
Query.ConfigureTo().DefineColumnName<Table>(t => t.ColumnName, "defined_column_name")
                   .DefineTableName<Table>("defined_table_name");
```

##### (NOTE) Naming Order
Defintion naming takes precedence over attribute naming which takes precedence over naming methods.

##### (NOTE) Columns Passed By String
By convention anytime a columnName is passed in via string it will not be passed through the configured naming methods. If required, you can call the string extension method `.ToColumnName()`.

### Many-To-Many Mappings
QueryForDapper allows you to define a mapping for join tables to make usage in queries easier.

```csharp
Query.ConfigureTo().MapManyToMany<Left,Join,Right>(join => join.leftId, join => join.rightId);
Query.ConfigureTo().MapManyToMany<Left,Join,Right>("leftId", "rightId");
```
##### (Note) requires matching column names 
Current implementation of `.MapManyToMany()` requires the column names to match between the left and right table and the join table.


Usage
=====

##### Currently Supported Keywords
* SELECT
* JOIN
* WHERE
* ORDER BY
* LIMIT
* OFFSET


To start a query chain:
```csharp
Query.FromTable<Table>()
```

## Select

##### (NOTE)  select all by default
If no select method as been called the resulting statement will have a `SELECT * FROM` by default.

##### Methods
| Extension Method | Result|
|------------------|-------|
|`Select<Table>()` | `SELECT Table.* FROM`|
|`Select<Table>("stringId")` | `SELECT Table.stringId FROM`|
|`Select<Table>(t => t.TableId)` | `SELECT Table.TableId FROM`|
|`Select<Table>("stringId", "stringName")` | `SELECT Table.stringId, Table.stringName FROM`|
|`Select<Table>(t => t.TableId, t => t.Name)` | `SELECT Table.TableId, Table.Name FROM`|

##### Example

```csharp
Query.FromTable<Authors>().Select<Authors>(a => a.FirstName, a.LastName)
                          .Select<Books>(b => b.Title);
```
result:
```sql
SELECT Authors.FirstName, Authors.LastName, Books.Title FROM Authors
```

## Join


##### Methods

| Extension Method | Result|
|------------------|-------|
|`.JoinOn<Table>(t => t.TableId)` | `INNER JOIN Table USING (TableId)` |
|`.JoinOn<Table>("table_id")` | `INNER JOIN Table USING (table_id)` |
|`.JoinOn<Left, Right>(l => l.LeftId, r => r.RightId)` | `INNER JOIN Right ON Left.LeftId = Right.RightId`|
|`.JoinOn<Left, Right>("left_id", "right_id")` | `INNER JOIN Right ON Left.left_id = Right.right_id`|



##### JoinTypes
The last parameter of each Join Method is `JoinType joinType = default`. The default join is INNER.

| JoinType | Result|
|------------------|-------|
`JoinType.inner`     |  `INNER`
`JoinType.LeftOuter` | `LEFT OUTER`
`JoinType.RightOuter`| `RIGHT OUTER`
`JoinType.FullOuter` | `FULL OUTER`

### JoinMany
If you defined a join map in configuration you can use the method:
```csharp
Query.ConfigureTo().MapManyToMany<LeftTable, JoinTable, RightTable>(j => j.LeftId, j => j.rightId);

Query.FromTable<Left>().JoinMany<Left,Right>()
                       .ToStatement();
```
result
```sql
SELECT * FROM LeftTable
INNER JOIN JoinTable USING (LeftId)
INNER JOIN RightTable USING (RightId)
```

## Where

### Operators
All where extensions methods have an operator parameter. `Operator @operator = default`. Default is none.

| Operator | Result|
|------------------|-------|
| `Operator.None` | BLANK |
| `Operator.And` | `AND` |
| `Operator.Or` | `OR` |
| `Operator.Not` | `NOT` |


### WhereCompared

##### Methods
| Extension Method | Result|
|------------------|-------|
|`.WhereCompared<Table>(t => t.Value, "CompareValue",Operator.None, Comparison.Equals)` | `WHERE Table.Value = 'ComparedValue'` |
|`.WhereComparedWith<Table>(t => t.Value, () => variable, Operator.None, Comparison.GreaterThan)` | `WHERE Table.Value > @variable` |

### Comparisons
WhereCompared methods have a comparision parameter. `Operator @operator = default`. Default is Equals.

| Operator | Result |
|------------------|-------|
| `Comparison.Equal` | `=` |
| `Comparison.NotEqual` | `<>` |
| `Comparison.LessThan` | `<` |
| `Comparison.LessThanEqual` | `<=` |
| `Comparison.GreaterThan` | `>` |
| `Comparison.GreaterThanEqual` | `>` |

##### Example

```csharp
public string QueryAuthor(int authorId)
{
    var query = Query.FromTable<Authors>.WhereComparedWith<Authors>(a => a.AuthorId, () => authorId)
                                        .JoinOn<Books>(b => b.AuthorId)
                                        .WhereCompared<Books>(b => b.Title, "BookTitle", Operator.And, Comparison.NotEqual)
                                        .ToStatement();
                                        ...
```
result
```sql
SELECT * FROM Authors
INNER JOIN Books USING (AuthorId)
WHERE Authors.AuthorId = @authorId AND Books.Title <> 'BookTitle'
```

### WhereLike

##### Methods
| Extension Method | Result|
|------------------|-------|
|`.WhereLike<Table>(t => t.Value, "searchValue")` | `WHERE Table.Value ILIKE '%' || 'ComparedValue' || '%'` |
|`.WhereLikeWith<Table>(t => t.Value, () => variable)` | `WHERE Table.Value ILIKE '%' || @variable || '%'` |

### Case
WhereLike methods have a Case parameter. `Case @case = default`. Default Is Insensitive.

| Operator | Result|
|------------------|-------|
| `Case.Insensitive` | `ILIKE` |
| `Case.Sensitive` | `LIKE` |

### Like
WhereLike methods have a Like parameter. `Like like = default`. Default Is Anywhere.

| Operator | Result|
|------------------|-------|
| `Like.Anywhere` | `'%' || {VALUE/VARIABLE} || '%'` |
| `Like.Begins` | `{VALUE/VARIABLE} || '%'` |
| `Like.Ends` | `'%' || {VALUE/VARIABLE}` |

##### Example

```csharp
var book = "Book";
Query.FromTable<Authors>.WhereLike<Authors>(a => a.Name, "Lastname", @case: Case.Sensitive, like: Like.Ends)
                        .WhereLikeWith<Authors>(a => a.LastBook, () => book, Operator.Or);
```
Result
```sql
SELECT * FROM Authors
WHERE Authors.Name LIKE '%' || 'Lastname' OR Authors.LastBook ILIKE '%' || @book || '%'
```

### WhereAnyWith
```csharp

public void QueryList(IEnuermable<Books> books)
{
    Query.FromTable<Books>().WhereAnyWith<Books>(b => b.Title, () => books)
```
result
```sql
SELECT * FROM Books
WHERE Books.Title = ANY(@books)
```

### WhereInSubQuery
```csharp
var subQuery = Query.FromTable<Authors>().WhereLike<Authors>(a => a.Name, "Smith", like: Like.Ends)
                                       .Select<Authors>(a => a.AuthorId);

Query.FromTable<Books>().WhereInSubQuery<Books>(b => b.AuthorId, subQuery);
```
result
```SQL
SELECT * FROM Books
WHERE Books.AuthorId IN (SELECT Authors.AuthorId FROM Authors WHERE Authors.Name ILIKE '%' || 'Smith')
```

## Order By
| Extension Method | Result|
|------------------|-------|
| `.OrderBy<Table>(t => t.TableId)` | `ORDER BY Table.TableId ASC` |
| `.OrderBy<Table>(t => t.TableId, Order.DESC)` | `ORDER BY Table.TableId DESC` |


### Order
OrderBy has a Order parameter. `Order order = default`. Default is `Order.ASC`

| Operator | Result|
|------------------|-------|
| `Order.ASC` | `ASC` |
| `Order.DESC` | `DESC` |

## Limit
```csharp
public void GetBooks(int count)
{
    Query.FromTable<Books>().TakeWith(() => count)
    ...
```
result
```sql
SELECT * FROM Books
LIMIT @count
```

## Offset
```csharp
public void GetBooks(int skip)
{
    Query.FromTable<Books>().SkipWith(() => skip)
    ...
```
result
```sql
SELECT * FROM Books
LIMIT @skip
```
