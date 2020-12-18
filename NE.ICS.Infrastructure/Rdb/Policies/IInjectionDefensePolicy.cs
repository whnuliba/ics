using System.Collections;

namespace NE.ICS.Infrastructure.Policies
{
    public interface IInjectionDefensePolicy
    {
        bool DetectInjectionWithParameters { get; set; }

        bool DetectInjection(string sql, IEnumerable parameters);

        bool DetectInjection(string whereClause, string orderBy, IEnumerable parameters);

    }
}
