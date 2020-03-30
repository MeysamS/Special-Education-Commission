using System;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;

namespace Comision.Service.ImplementationService
{
    public class TextDefaultService:ITextDefaultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITextDefaultRepository _textVoteService;
        public TextDefaultService(IUnitOfWork unitOfWork, ITextDefaultRepository textVoteService)
        {
            _unitOfWork = unitOfWork;
            _textVoteService = textVoteService;
        }
        public Tuple<bool, string> AddOrUpdate(TextDefault textDefault, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _textVoteService.Add(textDefault);
                else
                    _textVoteService.Update(textDefault);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public IQueryable<TextDefault> GetByType(long univercityId,TextDefaultType type)
        {
            return _textVoteService.Where(x =>x.UnivercityId==univercityId &&  x.TextDefaultType == type);
        }


        public IQueryable<TextDefault> GetVoteCommissionText()
        {
            return _textVoteService.Where(x => x.TextDefaultType == TextDefaultType.VoteCommissionText);
        }

        public IQueryable<TextDefault> GetVoteCouncilText()
        {
            return _textVoteService.Where(x => x.TextDefaultType == TextDefaultType.VoteCouncilText);
        }

        public IQueryable<TextDefault> GetProblemText()
        {
            return _textVoteService.Where(x => x.TextDefaultType == TextDefaultType.ProblemText);
        }

        public IQueryable<TextDefault> GetSpecialEducationVerdictText()
        {
            return _textVoteService.Where(x => x.TextDefaultType == TextDefaultType.SpecialEducationVerdictText);
        }       

        public Tuple<bool, string> Delete(long id)
        {
            try
            {
                _textVoteService.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }           
        }
    }
}
