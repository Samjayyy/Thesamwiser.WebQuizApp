INSERT INTO Answers (PlayerID,QuestionId,ChosenOptionIndex,AssignedValue, Status)
SELECT u.UserId, q.QuestionId, (1+ABS(CHECKSUM(NewId())) % 4), 1, 3 
FROM Users u,
	Questions q
WHERE NOT EXISTS (
	SELECT 1
	FROM Answers a
	WHERE a.PlayerID = u.UserId
		AND a.QuestionID = q.QuestionId
)

UPDATE a SET a.Status = 2
FROM Answers a
	INNER JOIN Questions q ON a.QuestionID = q.QuestionId
WHERE a.ChosenOptionIndex = q.IndexOfCorrectOption
	AND a.Status <> 2

