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
        var select = "<select class='ddlAttend form-control col-lg-2' name='attend'> <option value='Attending'>Attending</option><option value='Not Attending'>Not Attending</option></select>";
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

            $("#chkRoleList").remove();
            $("#chkRoles").append("<ul id='chkRoleList' class='col-md-9'  style='list-style-type:none;' ></ul>");
            $.each(data, function (i, item) {
                $("#chkRoleList").append("<li class='col-md-3'><span <label for='chk_Roles[]' class='checkbox-inline'><input type='checkbox' class='aria id='chkRole' name='chk_Roles[]' value='" + [data[i].Name] + "'/>" + data[i].Name + "</label></li>");
            });
        });
   //     <div class="col-lg-2 col-lg-offset-2">
//<div class="input-group">
  //<span class="input-group-addon">
      //  <input type="checkbox" aria-label="staffResidential">
    //</span>
  //  <span class="input-group-addon">Residential</span>
//</div>
        $("ddlMonth").on("change", function () {
            var month = $(this).val();
            var $url = "Meetings/LoadDays";
        });
        }
});