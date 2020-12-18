namespace NE.ICS.Infrastructure
{
    public static class ActionTypes
    {
        public const string Execute = nameof(Execute);

        public const string ExecuteAsync = nameof(ExecuteAsync);

        public const string ExecuteScalar = nameof(ExecuteScalar);

        public const string ExecuteScalarAsync = nameof(ExecuteScalarAsync);

        public const string Query = nameof(Query);

        public const string QueryAsync = nameof(QueryAsync);

        public const string QueryFirst = nameof(QueryFirst);

        public const string QueryFirstAsync = nameof(QueryFirstAsync);

        public const string QueryFirstOrDefault = nameof(QueryFirstOrDefault);

        public const string QueryFirstOrDefaultAsync = nameof(QueryFirstOrDefaultAsync);

        public const string QuerySingle = nameof(QuerySingle);

        public const string QuerySingleAsync = nameof(QuerySingleAsync);

        public const string QuerySingleOrDefault = nameof(QuerySingleOrDefault);

        public const string QuerySingleOrDefaultAsync = nameof(QuerySingleOrDefaultAsync);

        public const string QueryMultiple = nameof(QueryMultiple);

        public const string QueryMultipleAsync = nameof(QueryMultipleAsync);



        public const string Delete = nameof(Delete);

        public const string DeleteAsync = nameof(DeleteAsync);

        public const string DeleteAll = nameof(DeleteAll);

        public const string DeleteAllAsync = nameof(DeleteAllAsync);

        public const string Get = nameof(Get);

        public const string GetAsync = nameof(GetAsync);

        public const string GetAll = nameof(GetAll);

        public const string GetAllAsync = nameof(GetAllAsync);

        public const string Insert = nameof(Insert);

        public const string InsertAsync = nameof(InsertAsync);

        public const string Update = nameof(Update);

        public const string UpdateAsync = nameof(UpdateAsync);
    }
}
