-- Genereer inserts
SELECT 'INSERT INTO Question (Body, Option1, Option2, Option3, Option4, IndexOfCorrectOption, Comment, CreateAt) VALUES'

select '('''+REPLACE(ISNULL(Body,''),'''','''''')+''','''+REPLACE(ISNULL(Option1,''),'''','''''')+''','''+REPLACE(ISNULL(Option2,''),'''','''''')+''','''+REPLACE(ISNULL(Option3,''),'''','''''')+''','''+REPLACE(ISNULL(Option4,''),'''','''''')+''','''+CAST(IndexOfCorrectOption AS NVARCHAR(10))+''','''+REPLACE(ISNULL(Comment,''),'''','''''')+''', GETDATE()),'
from Questions

-- Migratie
SELECT COUNT(1) aantal
--UPDATE q SET q.OwnerUserId = (SELECT TOP 1 UserId FROM Users WHERE IsAdmin = 1)
FROM Questions q
WHERE q.OwnerUserId IS NULL
