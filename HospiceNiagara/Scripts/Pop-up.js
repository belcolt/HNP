$(function () {
    
    $('#pop-up').hide().css({ 'background-color': 'green', 'color': 'white', 'text-align':'center', 'vertical-align': 'middle' });
    $('#pop-up').css("position", "absolute");
    var moveLeft = 20;
    var moveDown = 20;
    //$('#hover').live('mouseover', function () {
    //    var $this = $(this);
    //    var $jid = $this.attr('jid');
    //    $('#pop-up').show();
    //    var $url = '/JobDescriptions/ShowDesc/' + $jid;

    //    $.ajax({
    //        method: 'POST',
    //        cache: false,
    //        url: $url,
    //        dataType: 'html',
    //        success: function (data) {
    //            //$('#pop-up').show();
    //            $('#pop-up').html(data);

    //        }
    //    });
    //});//.live('mouseleave', function () {
    //    $('#pop-up').hide();
    //});
    $('.hover').on("click", function (e) {
        var $this = $(this);
        e.preventDefault();
    });
    $('.hover').mousemove(function (e) {
        $("#pop-up").css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);
    });
    $('.hover').hover(function (e) {
        var $this = $(this);
        var $jid = $this.attr('jid');
        var $url = $this.attr('href');
            $.ajax({
                method: 'POST',
                cache: false,
                url: $url,
                dataType: 'html',
                success: function (data) {
                    $('#pop-up').show().css('top', e.pageY + moveDown)
                    .css('left', e.pageX + moveLeft)
                    .appendTo('body');
                    $('#pop-up').html(data);

                }
            });
           
        }, function () {
            $('#pop-up').hide();
        });

    
});