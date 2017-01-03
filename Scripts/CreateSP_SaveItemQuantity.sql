/****** Object:  StoredProcedure [Emerge].[SaveItemQuantity]    Script Date: 07/30/2014 16:46:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Deepak Artal
-- Create date: 12/25/2013
-- Description:	
-- =============================================
CREATE PROCEDURE [Emerge].[SaveItemQuantity] 
	@Quantity INT,	
	@ItemID INT,
	@OrderDestinationID INT
AS
BEGIN

	DECLARE @IsInvOrder INT
	SELECT @IsInvOrder = COUNT(*) FROM Emerge.tblOrder O
	LEFT JOIN Emerge.tblOrderDestination OD ON (OD.fk_Order_ID = O.pk_Order_ID)
	LEFT JOIN Emerge.tblOrderDestinationItem ODI ON ( ODI.fk_OrderDestination_ID = OD.pk_OrderDestination_ID)
	WHERE O.fk_Program_ID IS NULL
	AND OD.pk_OrderDestination_ID = @OrderDestinationID


	DECLARE @ApprovalRequired INT
	
	SELECT @ApprovalRequired = COUNT(*) FROM Emerge.tblItem I
	WHERE ((I.approval_required = 1) OR ((@Quantity > ISNULL(MaxOrderQuantity,0)) AND (ISNULL(MaxOrderQuantity,0) > 0)) )
	AND I.pk_Item_ID = @ItemID
	

	DECLARE @Exists INT
	
	SELECT @Exists = COUNT(*) FROM Emerge.tblOrderDestinationItem 
	WHERE fk_Item_ID = @ItemID AND fk_OrderDestination_ID = @OrderDestinationID

	IF @Exists = 0	-- INSERT
	BEGIN	
		IF @Quantity > 0 -- ONLY IF the Qty > 0
		BEGIN	
			IF( @ApprovalRequired > 0) AND  (@IsInvOrder >0)
			BEGIN
				INSERT INTO Emerge.tblOrderDestinationItem(fk_Item_ID, fk_OrderDestination_ID, quantity, ApprovedQuantity)
				VALUES(@ItemID, @OrderDestinationID, @Quantity, -1)
			END
			ELSE
			BEGIN
				INSERT INTO Emerge.tblOrderDestinationItem(fk_Item_ID, fk_OrderDestination_ID, quantity)
				VALUES(@ItemID, @OrderDestinationID, @Quantity)
			END
		END			
	END		
	ELSE				-- UPDATE
	BEGIN
		IF( @ApprovalRequired > 0) AND  (@IsInvOrder >0)
		BEGIN
			UPDATE Emerge.tblOrderDestinationItem
			SET quantity=@Quantity,
			ApprovedQuantity = -1
			WHERE fk_Item_ID=@ItemID
			AND fk_OrderDestination_ID=@OrderDestinationID
			IF @Quantity = 0
			BEGIN
				UPDATE Emerge.tblOrderDestinationItem SET ApprovedQuantity = NULL
				WHERE fk_Item_ID=@ItemID AND fk_OrderDestination_ID=@OrderDestinationID
			END
		END
		ELSE
		BEGIN
			UPDATE Emerge.tblOrderDestinationItem
			SET quantity=@Quantity,
			ApprovedQuantity = NULL
			WHERE fk_Item_ID=@ItemID
			AND fk_OrderDestination_ID=@OrderDestinationID
		END
			
	END	
END



