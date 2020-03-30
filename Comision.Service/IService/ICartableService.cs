using System;
using System.Collections.Generic;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface ICartableService
    {
        Tuple<bool, string, List<CartableViewModel>,ArgumentException> GetListCartable(long userId);
        int GetCartableCount(long userId);
        Tuple<bool, string, string> Confirmation(long userId, long personIdOnBehalfof, long requestId, long postuserId,
            int rowNumber, string description);
        Tuple<bool, string> Returned(long userId, long personIdOnBehalfof, long requestId, long postuserId,
            int rowNumber, string description);

        Tuple<bool, string> AgreetoVote(long userId, long personIdOnBehalfof, long requestId, long postuserId,
            int rowNumber, string description);

        Tuple<bool, string> OppositiontoVote(long userId, long personIdOnBehalfof, long requestId, long postuserId,
            int rowNumber, string description);

        Tuple<bool, string> VoteTemporary(VoteModel voteModel);
        Tuple<bool, string> VotePermanent(VoteModel voteModel);
        Tuple<bool, string, FollowUpModel> FollowUpRequest(long requestId);
        Tuple<bool, string, VoteModel> GetVote(long universityId, long voteId, RequestType requestType, AddressUrlFile addressUrlFile);
        Tuple<bool, string, Person> GetPersonInSigners(int rownumber, RequestType requestType, long fieldofStudyId);
        Tuple<bool, string, Person> GetPersonInSigners(long postId, long fieldofStudyId);
    }
}
