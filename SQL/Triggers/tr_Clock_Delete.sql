use [DisplayMonkey]
GO
/*******************************************************************
  2013-11-10 [DPA] - DisplayMonkey object
*******************************************************************/
alter trigger dbo.tr_Clock_Delete 
	on Clock 
	after delete
as
begin
	delete f from Frame f inner join deleted d on d.FrameId=f.FrameId
end
GO
