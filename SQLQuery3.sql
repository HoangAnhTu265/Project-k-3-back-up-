/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [User_Id]
      ,[UserName]
      ,[PassWord]
      ,[Email]
      ,[DOB]
      ,[Gender]
      ,[WorkStatus]
      ,[Address]
  FROM [SMSonline].[dbo].[Users]