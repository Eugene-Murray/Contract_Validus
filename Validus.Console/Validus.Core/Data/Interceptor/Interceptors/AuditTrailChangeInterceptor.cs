using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Text;
using System.Linq;

namespace Validus.Core.Data.Interceptor.Interceptors
{
    internal class AuditTrailChangeInterceptor : ChangeInterceptor<IAuditTrail>
    {
        private readonly List<string> _auditStrings = new List<string>();
        private readonly DateTime _auditTimeStamp = DateTime.UtcNow;

        public override void OnBeforeUpdate(DbContext dbContext, ObjectStateManager manager, IAuditTrail item)
        {

            var entry = manager.GetObjectStateEntry(item);
            var auditString = new StringBuilder();
            auditString.AppendFormat("Entity Name {0} :UPDATED:ORGINAL: ", item.GetType());
            foreach (var propName in entry.GetModifiedProperties())
            {

                auditString.AppendFormat("{0}", propName);
                auditString.AppendFormat("=>{0}:", entry.OriginalValues[propName]);
            }
            auditString.Append(":MODIFIED: ");
            foreach (var propName in entry.GetModifiedProperties())
            {

                auditString.AppendFormat("{0}", propName);
                auditString.AppendFormat("=>{0}:", entry.CurrentValues[propName]);
            }

            _auditStrings.Add(auditString.ToString());
        }

        public override void OnBeforeDelete(DbContext dbContext, ObjectStateManager manager, IAuditTrail item)
        {

            var entry = manager.GetObjectStateEntry(item);
            var auditString = new StringBuilder();
            auditString.AppendFormat("Entity Name {0} :DELETED: ", item.GetType());
            foreach (var propName in entry.GetModifiedProperties())
            {

                auditString.AppendFormat("{0}", propName);
                auditString.AppendFormat("=>{0}:", entry.OriginalValues[propName]);
            }

            _auditStrings.Add(auditString.ToString());
        }

        public override void OnAfterInsert(DbContext dbContext, ObjectStateManager manager, IAuditTrail item)
        {

            var entry = manager.GetObjectStateEntry(item);
            var auditString = new StringBuilder();
            auditString.AppendFormat("Entity Name {0} :INSERTED: ", item.GetType());
        

            foreach( var prop in item.GetType().GetProperties())
            {
             
                    try
                    {
                        var val = entry.OriginalValues[prop.Name];
                        auditString.AppendFormat("{0}", prop.Name);
                        auditString.AppendFormat("=>{0}:", val);
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                    }
            }

            _auditStrings.Add(auditString.ToString());
        }


        public override void OnBeforeDeleteRelationship(DbContext dbContext, ObjectStateManager manager, Object relation, params Object[] items)
        {

            var auditString = new StringBuilder();
            auditString.AppendFormat("Entity Name {0} :DELETED BETWEEN ", (((System.Data.Objects.ObjectStateEntry)(relation)).EntitySet).Name);
            foreach (dynamic item in items)
            {
                auditString.AppendFormat("Entity Name {0} ID=>{1}", item.GetType(), item.Id);
            }
            _auditStrings.Add(auditString.ToString());
        }

        public override void OnAfterInsertRelationship(DbContext dbContext, ObjectStateManager manager, Object relation, params Object[] items)
        {
            var auditString = new StringBuilder();
            auditString.AppendFormat("Entity Name {0} :ADDED BETWEEN ", (((System.Data.Objects.ObjectStateEntry)(relation)).EntitySet).Name);
            foreach (dynamic item in items)
            {
                auditString.AppendFormat("Entity Name {0} ID=>{1}", item.GetType(), item.Id);
            }
            _auditStrings.Add(auditString.ToString());

        }

        public string AuditDump()
        {
            if (_auditStrings.Any())
            {
                var auditDump = new StringBuilder();
                auditDump.AppendFormat("DB Audit TimeStamp{0} :", this._auditTimeStamp.ToUniversalTime());
                auditDump.Append(Environment.NewLine);
                auditDump.Append(_auditStrings.Aggregate((current, next) => current + Environment.NewLine + next));
                return auditDump.ToString();
            }
            else
            {
                var auditDump = new StringBuilder();
                auditDump.AppendFormat("TimeStamp{0} :", this._auditTimeStamp.ToUniversalTime());
                return auditDump.ToString();

            }

        }
    }

}
