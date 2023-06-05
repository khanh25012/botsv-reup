
-- =============================================
alter FUNCTION fn_LinkGPS
(
	@lat float,
	@lng float
)
RETURNS nvarchar(max)
AS
BEGIN
	declare @url nvarchar(max)='https://www.google.com/maps/search/';

	set @url += ltrim(str(@lat,10,6)) + ',' + ltrim(str(@lng,10,6));
	
	declare @link nvarchar(max);
	set @link = '<a href="'+@url+'" >Click</a>';
	RETURN @link;
END
GO

-- =============================================
-- Author:		do duy cop
-- Create date: 25.5.2023
-- Description:	thủ tục tìm kiếm
-- =============================================
ALTER proc [dbo].[SP_Search]
	@q nvarchar(50)=''
as 
BEGIN
	declare @br char(2)=char(10)+char(13);  -- để xuống dòng
	declare @kq nvarchar(max)='';  -- để chứa kq cuối cùng
	declare @stt int =1;

	--where giúp tìm thấy nhiều bản ghi (hoặc 1, hoặc 0)
	--select @kq +=  : nhấn mạnh vào +=  : ghép chuỗi vào sau @kq đã có
	-- khi select có nhiều bản ghi, ghi mỗi bản ghi đc tạo ra chuỗi, ghép vào sau @kq
	-- khi chạy xong select -> chỉ tạo ra 1 chuỗi @kq dài dài
	select @kq += FORMATMESSAGE(N'👉Kết quả số: %d%sid: %d%s%s: %s%sGPS: %s, test giá mới: 99$, giá cũ: <del>120$</del>   %s%s📌Google Map: %s%s',
					@stt,@br, id,@br, loai, ten, @br,str(lat,10,6),str(lng,10,6),@br, dbo.fn_LinkGPS(lat,lng),@br+@br),
	       @stt=@stt+1
	from DiaDanh 
	where ten like @q;

	if(@kq='' or @kq is null)
		set @kq=N'🤪Không tìm thấy gì liên quan';
	else
		set @kq+=N'😍Kết luận đã tìm thấy '+ltrim(str(@stt,3))+N' kết quả'

	select @kq as msg;  -- trả về 1 dòng, 1 cột tên là MSG
END