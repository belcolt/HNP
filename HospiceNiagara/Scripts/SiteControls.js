$(function () {

    $("#resCatDD").on("change", function () {
       getSubs($(this).val());
    });

    function getSubs(catID) {
        $.getJSON("/Resources/LoadSubcategories","catID="+catID,function (data) {
            $(".subCatGen").remove();
            $.each(data, function (i, item) {
                $("#resSubCatDD").append("<option class=subCatGen value=" + data[i].ID + ">" + data[i].Name + "</option>");
            });
        });
    };

    $(".btnRSVP").click(function () {
        $(this).hide();
        var $btn = $(this);
        var select = "<select class='ddlAttend' name='attend'> <option value='Attending'>Attending</option><option value='Not Attending'>Not Attending</option></select>";
        select+="<button type='button' class='btn btn-default, btnReply'>Reply</button>";
        
        $(this).parent().append(select);
    });

    $("td").delegate(".btnReply", "click", changeAttend);
    $("#chkRole").on("change")
    {
        var $chks = $('.chkSubRole:checked');
        $.each($chks,function(i, item)
        {

        })
    }
    function changeAttend()
    {
        var $select = $(this).prev();
        var $statBtn = $select.prev().prev();
        //alert ($(this).prev().val());
        var $reply = $(this);
        var attend = $(this).prev().val();
        var inviteID = $(this).prev().prev().val();
        $url = "/Meetings/AttendResponse?inviteID=" + inviteID+ "attend=" + attend;
        $.ajax(
            {
                method: "POST",
                url: "/Meetings/AttendResponse?",
                data: {
                    'inviteID' : inviteID,
                    'attend' : attend
                },
                success:function()
        {
                    $statBtn.val(attend);
                    $statBtn.show();
                    $select.remove();
                    $reply.remove();
        }

    });
    }

    $("#ddlTeam").on("change",null,null,LoadRoles);
        function LoadRoles()
        {
        var domainID = $(this).val();
        var $url = "/Admin/LoadRoles";
        $.getJSON($url, 'domain='+domainID, function (data) {

            $(".chkDiv").remove();
            $.each(data, function (i, item) {
                $("#chkRoles").append("<div class='col-md-10, chkDiv'><input type='checkbox' id='chkRole' name='chk_Roles[]' class='chksubRole' value=" + data[i].Name + ">" + data[i].Name + "</div>");
            });
        });
            //$.ajax(
            //    {
            //        method: "GET",
            //        url: $url,
            //        type: JSON,
            //        success: function (data) {
            //        }
            //    });
        }
});