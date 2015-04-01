$(function () {
    var $currPage = window.location.href;
    
    if ($currPage.indexOf("Schedules/Create") > -1) {

        $("select").on("change", function (evt) {
            $this = $(this);
            
            if ($("#SchedMonth").val() != "" & $("#SchedYear").val() != "" & $("#ResourceSubcategory").val() != "") {
                $month = $("#SchedMonth").val();
                $year = $("#SchedYear").val();
                $category = $("#ResourceSubcategory").val();
                $url = '/Schedules/CheckSchedule?month=' + $month + '&year=' + $year + '&category=' + $category;

                $.ajax({
                    method: "POST",
                    cache: false,
                    url: $url,
                    dataType: 'html',
                    success: function (data) {
                        if (data.length > 0) {
                            $('#someDiv').html(data);
                        }
                        else {
                            $('#someDiv').html("");
                        }


                    }
                });
            }
        });
    }
    if ($currPage.indexOf("Schedules/Edit") > -1) {

        $start = $("#IsActiveSchedule").is(':checked');
        $("#SchedYear").val($('#Year').val());
        $("#SchedMonth").val($('#Month').val());
        $("#IsActiveSchedule").on("click", function (evt) {
            if ($("#IsActiveSchedule").is(':checked') != $start) {
                $this = $(this);
                $year = $("#SchedYear").val();
                $month = $("#SchedMonth").val();
                $category = $("#Category").val();
                $id = $("#ID").val();
                $url = '/Schedules/CheckEditSchedule?month=' + $month + '&year=' + $year + '&id=' + $id + '&category=' + $category + '&start=' + $start;

                $.ajax({
                    method: "POST",
                    cache: false,
                    url: $url,
                    dataType: 'html',
                    success: function (data) {
                        if (data.length > 0) {
                            $('#someDiv').html(data);
                        }
                        else {
                            $('#someDiv').html("");
                        }


                    }
                });
            }
            else {
                $('#someDiv').html("");
            }
            
        });
    }
});