using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer.Models;
using TestDelivery.Common.Extensions;
namespace TestDelivery.ServiceLayer
{
    public class TemplateService : ITemplateService
    {

        public List<TestTemplate> ListTemplates()
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from template in context.TestTemplate.OrderByDescending(t => t.IsActive).ThenByDescending(t => t.Priority).ThenBy(t => t.Name)
                        select template).ToList();
            }
        }


        public void AddTemplate(TestTemplate template)
        {
            template.Id = Guid.NewGuid();
            using (var context = new TestDeliveryEntities())
            {
                context.TestTemplate.AddObject(template);
                context.SaveChanges();
            }
        }


        public TestTemplate GetTemplateById(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from template in context.TestTemplate
                        where template.Id == id
                        select template).FirstOrDefault();
            }
        }


        public void DeleteTemplate(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                TestTemplate template = (from temp in context.TestTemplate
                                         where temp.Id == id
                                         select temp).FirstOrDefault();
                context.DeleteObject(template);
                context.SaveChanges();
            }
        }


        public List<TestTemplate> GetActiveTemplates()
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from template in context.TestTemplate.Include(t=>t.Tests)
                        where template.IsActive == true
                        select template).OrderBy(t => t.Priority).ToList();
            }
        }

        public void DeleteTests(Guid templateId)
        {
            using (var context = new TestDeliveryEntities())
            {
                var tests = context.TestTemplate
                       .Where(t => t.Id == templateId)
                       .First()
                       .Tests;

                foreach (var test in tests)
                    context.Test.DeleteObject(test);
            }
        }

        public TestTemplateExtraInfo GetExtraInfo(Guid templateId)
        {
            using (var context = new TestDeliveryEntities())
            {
                return context.TestTemplate
                       .Where(t => t.Id == templateId)
                       .Select(t => new TestTemplateExtraInfo
                       {
                           Template = t,
                           TestCount = t.Tests.Count()
                       })
                       .First();
            }
        }

        public void SaveTemplate(TestTemplate template)
        {
            using (var context = new TestDeliveryEntities())
            {
                context.TestTemplate.Attach(template);
                context.ObjectStateManager.ChangeObjectState(template, System.Data.EntityState.Modified);
                context.SaveChanges();
            }
        }


        public TestTemplate CopyTemplate(TestTemplate testToCopy)
        {
            var copy = testToCopy.DeepCloneWithSerialization();
            copy.Name += " Copy";
            copy.IsActive = false;
            AddTemplate(copy);
            return copy;
        }

    }

    public interface ITemplateService
    {
        List<TestTemplate> ListTemplates();
        void AddTemplate(TestTemplate template);
        TestTemplate GetTemplateById(Guid id);
        void DeleteTemplate(Guid id);
        List<TestTemplate> GetActiveTemplates();
        void DeleteTests(Guid templateId);
        TestTemplateExtraInfo GetExtraInfo(Guid templateId);
        void SaveTemplate(TestTemplate template);
        TestTemplate CopyTemplate(TestTemplate testToCopy);
    }
}
