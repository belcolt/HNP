$(function () {
    if ($("#ContactsAddID").val() == "") {
        $url = "/Meetings/LoadDropDown/";

        $.ajax({
            method: 'POST',
            cache: false,
            url: $url,
            dataType: 'html',
            success: function (data) {
                $('#someDiv').html(data);

            }
        });
    }
    $("#ContactsAddID").on("change", function (evt) {
        $this = $(this);
        if ($this.val() != "") {
            $id = $this.val();
            $url = "/Meetings/LoadDropDown/" + $this.val();

            $.ajax({
                method: 'POST',
                cache: false,
                url: $url,
                dataType: 'html',
                success: function (data) {
                    $('#someDiv').html(data);

                }
            });
        }
        if ($this.val() == "") {
            $url = "/Meetings/LoadDropDown/";

            $.ajax({
                method: 'POST',
                cache: false,
                url: $url,
                dataType: 'html',
                success: function (data) {
                    $('#someDiv').html(data);

                }
            });
        }
    });

});