using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.Common;
using TestDelivery.DAL;

namespace TestDelivery.ServiceLayer
{
    public class RespondentService : IRespondentService
    {
        public List<Respondent> ListRespondents()
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from respondent in context.Respondent.Include(t=>t.Tests)
                        select respondent).ToList();
            }
        }
        public ITemplateService TemplateService { get; set; }
        public ITestService TestService { get; set; }

        public void AddRespondent(Respondent respondent)
        {
            respondent.Id = Guid.NewGuid();
            respondent.SecretCode = SecretCodeGenerator.GetNewCode();

            using (var context = new TestDeliveryEntities())
            {
                context.Respondent.AddObject(respondent);
                context.SaveChanges();
            }

            CreateInitialTests(respondent);
        }

        private void CreateInitialTests(Respondent respondent)
        {
            var tests = TemplateService.GetActiveTemplates()
                                        .Select(template => new Test
                                        {
                                            Answers = new List<Answer>(),
                                            RespondentId = respondent.Id,
                                            TestId = template.Id,
                                        });

            foreach (var test in tests)
                TestService.AddTest(test);
        }

        public void DeleteRespondent(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                Respondent respondent = (from resp in context.Respondent
                                         where resp.Id == id
                                         select resp).FirstOrDefault();
                context.DeleteObject(respondent);
                context.SaveChanges();
            }
        }


        public Respondent GetRespondentById(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from respondent in context.Respondent
                        where respondent.Id == id
                        select respondent).FirstOrDefault();
            }
        }

        public Respondent GetRespondentBySecretCode(string code)
        {
            using (var context = new TestDeliveryEntities())
            {
                return context.Respondent.Where(r => r.SecretCode == code).FirstOrDefault();
            }
        }

        public void SaveRespondent(Respondent respondent)
        {
            using (var context = new TestDeliveryEntities())
            {
                context.Respondent.Attach(respondent);
                context.ObjectStateManager.ChangeObjectState(respondent, System.Data.EntityState.Modified);
                context.SaveChanges();
            }
        }

    }

    public interface IRespondentService
    {
        List<Respondent> ListRespondents();
        void AddRespondent(Respondent respondent);
        void DeleteRespondent(Guid id);
        Respondent GetRespondentById(Guid id);
        Respondent GetRespondentBySecretCode(string code);
        void SaveRespondent(Respondent respondent);
    }
}
