
/****** Object:  UserDefinedFunction [Emerge].[fn_GetAvailableStockByItemID]    Script Date: 07/30/2014 12:19:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [Emerge].[fn_GetAvailableStockByItemID] 
(
	-- Add the parameters for the function here
	@ItemID INT,
	@OrderID INT
)
RETURNS INT
AS
BEGIN
	DECLARE @AvailableInventory AS INT
	
			
	SELECT @AvailableInventory = ISNULL(Stock,0) FROM Emerge.tblItem WHERE pk_Item_ID = @ItemID
	
	
	SELECT @AvailableInventory = (ISNULL(Stock,0) - ISNULL(ItemsInOrders.StockBlocked,0))  FROM Emerge.tblItem  I
		LEFT JOIN (SELECT ODI.fk_Item_ID, SUM(ODI.quantity) StockBlocked FROM Emerge.tblOrder O
		LEFT JOIN Emerge.tblOrderDestination OD ON ( OD.fk_Order_ID = O.pk_Order_ID) AND O.pk_Order_ID != @OrderID
		LEFT JOIN Emerge.tblOrderDestinationItem ODI ON ( ODI.fk_OrderDestination_ID = OD.pk_OrderDestination_ID)
		WHERE O.fk_Program_ID IS NULL AND O.Status IN ('OPEN', 'PENDAPPROVAL','IN PROCESS') AND ODI.pk_OrderDestinationItem_ID IS NOT NULL
		GROUP BY ODI.fk_Item_ID) ItemsInOrders ON ( ItemsInOrders.fk_Item_ID = I.pk_Item_ID)

		WHERE pk_Item_ID = @ItemID
	
	RETURN @AvailableInventory 
END


GO


