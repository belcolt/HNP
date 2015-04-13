$(function () {
    $("#ContactsAddID option").first().val("All");
  
    if ($("#ContactsAddID").val() == "All") {

        var cIDs = [];
        $("#ChoosenID option").each(function () {
            cIDs.push($(this).val());
        })
       
        $url = "/Meetings/LoadDropDown?dID=" + "&cIDs=" + cIDs.join(",");

        $.ajax({
            method: 'POST',
            cache: false,
            url: $url,
            dataType: 'html',
            success: function (data) {
                $('#someDiv').html(data);
                $("#ChooseList option").first().val("All");
            }
        });
    }
   
    $("#ContactsAddID").on("change", function (evt) {
        $this = $(this);
        if ($this.val() == "All") {
            $("#ChooseList option").first().val("All");
        }
        //if ($this.val() != "") {
            $id = $this.val();
            var cIDs = [];
            $("#ChoosenID option").each(function(){
                 cIDs.push($(this).val());
            })
            
            $url = "/Meetings/LoadDropDown?dID=" + $this.val() + "&cIDs=" + cIDs.join(",");
            
            
            
            $.ajax({
                method: 'POST',
                cache: false,
                url: $url,
                dataType: 'html',
                success: function (data) {
                    $('#someDiv').html(data);
                    if ($("#ContactsAddID :selected").val() != "All") {
                        $('#ChooseList option').first().text('All ' + $("#ContactsAddID :selected").text() + ' Contacts').val($("#ContactsAddID :selected").text());
                    }
                    else {
                        $("#ChooseList option").first().val("All");
                    }
                   
                    
                }
            });
        //}
        //if ($this.val() == "") {
        //    $url = "/Meetings/LoadDropDown/";

        //    $.ajax({
        //        method: 'POST',
        //        cache: false,
        //        url: $url,
        //        dataType: 'html',
        //        success: function (data) {
        //            $('#someDiv').html(data);

        //        }
        //    });
        //}
    });
    $(document).on("click", '#add', function (e) {
        $("#ChooseList option:selected").each(function () {
            //alert($(this).val() + $(this).text());
            if ($(this).val() == "All") {
                $('#ChoosenID option').each(function () {
                    $(this).remove().appendTo("#ChooseList");
                });
                $(this).remove().appendTo('#ChoosenID');
                $("#ChooseList").prop("disabled", true);
                return false;
            }
            else {
                if ($("#ChoosenID option").first().val() == "All") {
                    return false;
                }
                else {
                    $(this).remove().appendTo('#ChoosenID');
                }
                
            }
            
         })
    });
    $(document).on("click", '#remove', function (e) {
        $("#ChoosenID option:selected").each(function () {
            //alert($(this).val() + $(this).text());
            if ($(this).val() == "All") {
                $("#ChooseList").prop("disabled", false);
            }
            $(this).remove().appendTo('#ChooseList');
        })
    });
});