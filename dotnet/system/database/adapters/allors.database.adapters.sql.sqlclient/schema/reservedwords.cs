// <copyright file="ReservedWords.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient
{
    using System.Collections.Generic;

    /// <summary>
    /// As of Sql Server 2008 R2.
    /// </summary>
    internal static class ReservedWords
    {
        internal static readonly HashSet<string> Names;

        internal static readonly string[] Current =
        {
                                             "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP",
                                             "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE",
                                             "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLUMN",
                                             "COMMIT", "COMPUTE", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE",
                                             "CONVERT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME",
                                             "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC",
                                             "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK",
                                             "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMP", "ELSE", "END", "ERRLVL",
                                             "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL", "FETCH",
                                             "FILE", "FILLFACTOR", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM",
                                             "FULL", "FUNCTION", "GOTO", "GRANT", "GROUP", "HAVING", "HOLDLOCK",
                                             "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER",
                                             "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY", "KILL", "LEFT", "LIKE",
                                             "LINENO", "LOAD", "MERGE", "NATIONAL", "NOCHECK ", "NONCLUSTERED", "NOT",
                                             "NULL", "NULLIF", "OF", "OFF", "OFFSETS", "ON", "OPEN", "OPENDATASOURCE",
                                             "OPENQUERY", "OPENROWSET", "OPENXML", "OPTION", "OR", "ORDER", "OUTER",
                                             "OVER", "PERCENT", "PIVOT", "PLAN", "PRECISION", "PRIMARY", "PRINT", "PROC",
                                             "PROCEDURE", "PUBLIC", "RAISERROR", "READ", "READTEXT", "RECONFIGURE",
                                             "REFERENCES", "REPLICATION", "RESTORE", "RESTRICT", "RETURN", "REVERT",
                                             "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE",
                                             "SCHEMA", "SECURITYAUDIT", "SELECT", "SESSION_USER", "SET", "SETUSER",
                                             "SHUTDOWN", "SOME", "STATISTICS", "SYSTEM_USER", "TABLE", "TABLESAMPLE",
                                             "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE",
                                             "TSEQUAL", "UNION", "UNIQUE", "UNPIVOT", "UPDATE", "UPDATETEXT", "USE",
                                             "USER", "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE",
                                             "WITH", "WRITETEXT",
                                         };

        internal static readonly string[] Future =
        {
                                                     "ABSOLUTE", "ACTION", "ADMIN", "AFTER", "AGGREGATE", "ALIAS",
                                                     "ALLOCATE", "ARE", "ARRAY", "ASENSITIVE", "ASSERTION", "ASYMMETRIC",
                                                     "AT", "ATOMIC", "BEFORE", "BINARY", "BIT", "BLOB", "BOOLEAN", "BOTH",
                                                     "BREADTH", "CALL", "CALLED", "CARDINALITY", "CASCADED", "CAST",
                                                     "CATALOG", "CHAR", "CHARACTER", "CLASS", "CLOB", "COLLATION",
                                                     "COLLECT", "COMPLETION", "CONDITION", "CONNECT", "CONNECTION",
                                                     "CONSTRAINTS", "CONSTRUCTOR", "CORR", "CORRESPONDING", "COVAR_POP",
                                                     "COVAR_SAMP", "CUBE", "CUME_DIST", "CURRENT_CATALOG",
                                                     "CURRENT_DEFAULT_TRANSFORM_GROUP", "CURRENT_PATH", "CURRENT_ROLE",
                                                     "CURRENT_SCHEMA", "CURRENT_TRANSFORM_GROUP_FOR_TYPE", "CYCLE", "DATA",
                                                     "DATE", "DAY", "DEC", "DECIMAL", "DEFERRABLE", "DEFERRED", "DEPTH",
                                                     "DEREF", "DESCRIBE", "DESCRIPTOR", "DESTROY", "DESTRUCTOR",
                                                     "DETERMINISTIC", "DICTIONARY", "DIAGNOSTICS", "DISCONNECT", "DOMAIN",
                                                     "DYNAMIC", "EACH", "ELEMENT", "END-EXEC", "EQUALS", "EVERY",
                                                     "EXCEPTION", "FALSE", "FILTER", "FIRST", "FLOAT", "FOUND", "FREE",
                                                     "FULLTEXTTABLE", "FUSION", "GENERAL", "GET", "GLOBAL", "GO",
                                                     "GROUPING", "HOLD", "HOST", "HOUR", "IGNORE", "IMMEDIATE",
                                                     "INDICATOR", "INITIALIZE", "INITIALLY", "INOUT", "INPUT", "INT",
                                                     "INTEGER", "INTERSECTION", "INTERVAL", "ISOLATION", "ITERATE",
                                                     "LANGUAGE", "LARGE", "LAST", "LATERAL", "LEADING", "LESS", "LEVEL",
                                                     "LIKE_REGEX", "LIMIT", "LN", "LOCAL", "LOCALTIME", "LOCALTIMESTAMP",
                                                     "LOCATOR", "MAP", "MATCH", "MEMBER", "METHOD", "MINUTE", "MOD",
                                                     "MODIFIES", "MODIFY", "MODULE", "MONTH", "MULTISET", "NAMES",
                                                     "NATURAL", "NCHAR", "NCLOB", "NEW", "NEXT", "NO", "NONE", "NORMALIZE",
                                                     "NUMERIC", "OBJECT", "OCCURRENCES_REGEX", "OLD", "ONLY",
                                                     "OPERATION", "ORDINALITY", "OUT", "OVERLAY", "OUTPUT", "PAD",
                                                     "PARAMETER", "PARAMETERS", "PARTIAL", "PARTITION", "PATH", "POSTFIX",
                                                     "PREFIX", "PREORDER", "PREPARE", "PERCENT_RANK", "PERCENTILE_CONT",
                                                     "PERCENTILE_DISC", "POSITION_REGEX", "PRESERVE", "PRIOR",
                                                     "PRIVILEGES", "RANGE", "READS", "REAL", "RECURSIVE", "REF",
                                                     "REFERENCING", "REGR_AVGX", "REGR_AVGY", "REGR_COUNT",
                                                     "REGR_INTERCEPT", "REGR_R2", "REGR_SLOPE", "REGR_SXX", "REGR_SXY",
                                                     "REGR_SYY", "RELATIVE", "RELEASE", "RESULT", "RETURNS", "ROLE",
                                                     "ROLLUP", "ROUTINE", "ROW", "ROWS", "SAVEPOINT", "SCROLL", "SCOPE",
                                                     "SEARCH", "SECOND", "SECTION", "SENSITIVE", "SEQUENCE", "SESSION",
                                                     "SETS", "SIMILAR", "SIZE", "SMALLINT", "SPACE", "SPECIFIC",
                                                     "SPECIFICTYPE", "SQL", "SQLEXCEPTION", "SQLSTATE", "SQLWARNING",
                                                     "START", "STATE", "STATEMENT", "STATIC", "STDDEV_POP", "STDDEV_SAMP",
                                                     "STRUCTURE", "SUBMULTISET", "SUBSTRING_REGEX", "SYMMETRIC", "SYSTEM",
                                                     "TEMPORARY", "TERMINATE", "THAN", "TIME", "TIMESTAMP",
                                                     "TIMEZONE_HOUR", "TIMEZONE_MINUTE", "TRAILING", "TRANSLATE_REGEX",
                                                     "TRANSLATION", "TREAT", "TRUE", "UESCAPE", "UNDER", "UNKNOWN",
                                                     "UNNEST", "USAGE", "USING", "VALUE", "VAR_POP", "VAR_SAMP", "VARCHAR",
                                                     "VARIABLE", "WHENEVER", "WIDTH_BUCKET", "WITHOUT", "WINDOW",
                                                     "WITHIN", "WORK", "WRITE", "XMLAGG", "XMLATTRIBUTES", "XMLBINARY",
                                                     "XMLCAST", "XMLCOMMENT", "XMLCONCAT", "XMLDOCUMENT", "XMLELEMENT",
                                                     "XMLEXISTS", "XMLFOREST", "XMLITERATE", "XMLNAMESPACES", "XMLPARSE",
                                                     "XMLPI", "XMLQUERY", "XMLSERIALIZE", "XMLTABLE", "XMLTEXT",
                                                     "XMLVALIDATE", "YEAR", "ZONE",
                                                 };

        static ReservedWords()
        {
            Names = new HashSet<string>();
            foreach (var name in Current)
            {
                Names.Add(name.ToLowerInvariant());
            }

            foreach (var name in Future)
            {
                Names.Add(name.ToLowerInvariant());
            }
        }
    }
}
