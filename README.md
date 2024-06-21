## My Restaurant

**System requirements**

1. .NET 8 SDK
2. PostgresSQL.
3. Before running the application please run migrations.

## Quick note

1. Moved OnArrived method into Customer service as its responsibility.
2. Restaurant tables are shared resources hence serialization isolation level is applied.

