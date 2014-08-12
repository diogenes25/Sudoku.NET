function Cell() {
    iCandidateValueInternal = 0;
    id;
    iHouseType;
    iCandidateValue;
		Digit = 0;

		HouseCollection[] fieldcontainters = new HouseCollection[3];

		function Set_CandidateValue(inCandidateValue)
		{
iCandidateValue = inCandidateValue;
				if (iCandidateValue > 0 && iDigit > 0 && inCandidateValue > 0)
					Digit = 0;
		}

		function Set_Digit(inDigit)
		{
					if (iDigit ==inDigit)
						return;
					if (inDigit> 0 && inDigit<= Consts.DimensionSquare && Digit < 1 && (iCandidateValue & (1 << (inDigit- 1))) == (1 << (inDigit- 1)))
					{
iDigit = inDigit;
							iCandidateValue = 0;
					}
		}
}