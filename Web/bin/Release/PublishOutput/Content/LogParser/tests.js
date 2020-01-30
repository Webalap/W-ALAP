define([
    "latex-log-parser",
    "bib-log-parser",
    "jquery"
],
    function (LatexParser, BibLogParser, $) {
        $("#btnCompile").on("click", function () {
            $('#loading').show();
            $.ajax({
                url: 'CompileLatex',
                type: "POST",
                data: {
                    DocumentText: editor.getValue()
                },
                success: function (data) {
                    $('#loading').hide();
                    if (data.Status) {
                        $('#colErrors').find('div').remove();
                        $("#colErrors").append('<div id="pdfViewer"></div>');
                        PDFObject.embed(data.FileName, "#pdfViewer");
                        toastr.success('Latex file is compiled.');
                    }
                    else {
                        PDFObject.embed(data.FileName, "#pdfViewer");
                        toastr.error('Found Errors in latex file.');
                        // Populate Error
                        var errors = LatexParser.parse(data.LogFileName, {
                            ignoreDuplicates: false
                        }).errors;
                        $("#pdfViewer").hide();
                        populateErrors(errors);
                    }
                },
                error: function (result) {
                    $('#loading').hide();
                    toastr.error(result.statusText);
                }
            });
        });
        var populateErrors = function (errors) {
            $('#colErrors').find('div').remove();
            var htmlToDisplay = '<div><table class=\"table\"><thead><tr><td>Count</td><td>Message</td><td>Refernce</td></thead></tr>';
            for (var i = 0; i < errors.length; i++) {
                var errorMessage = errors[i].content.split('<*>')[0];
                var contentMessage = $.trim(errorMessage.split('<inserted text>')[0]);
                var refernceMessage = $.trim(errorMessage.split('<inserted text>')[1]);

                if (contentMessage === '' || refernceMessage === '') {
                    errorMessage = errors[i].content.split("Here is how much of TeX's memory you used")[0];
                    var lineNumber = errorMessage.split('\\');
                    contentMessage = 'Misspelled on line# ' + lineNumber[0].split('.')[1] + ' latex tag ' + lineNumber[1];
                    if (errors[i].message !== ' ==> Fatal error occurred, no output PDF file produced!') {
                        htmlToDisplay = htmlToDisplay + '<tr><td>' + (i + 1) + '</td><td onclick="someError(this)" class="tableForErrors">' + contentMessage + '</td><td>' + errorMessage + '</td></tr>';
                        //'<div class=\"alert alert-danger\"><strong> Content: </strong > ' + errorMessage + '</div>'
                        //+'<div class=\"alert alert-danger\"><strong> Message </strong > ' + errors[i].message+'</div>'
                        //+'<div class=\"alert alert-danger\"><strong> File </strong > ' + errors[i].file+'</div>'
                    }
                }
                else {
                    if (errors[i].message !== ' ==> Fatal error occurred, no output PDF file produced!') {
                        var genratedErrors = GenrateRefernceErrors(refernceMessage);
                        var jsonObject = { "Error": { "Message": genratedErrors } };
                        htmlToDisplay = htmlToDisplay + '<tr><td>' + (i + 1) + '</td><td onclick="someError(this)" class="tableForErrors" name="' + jsonObject.Error.Message+'">' + jsonObject.Error.Message + '</td><td>' + contentMessage + '</td></tr>';
                        //'<div class=\"alert alert-danger\"><strong> Content: </strong > ' + errorMessage + '</div>'
                        //+'<div class=\"alert alert-danger\"><strong> Message </strong > ' + errors[i].message+'</div>'
                        //+'<div class=\"alert alert-danger\"><strong> File </strong > ' + errors[i].file+'</div>'
                    }
                }
            }
            htmlToDisplay = htmlToDisplay + '</table></div>';
            $("#colErrors").append(htmlToDisplay);
        }
    
        var GenrateRefernceErrors = function (stringToConvert)
        {
            if ('\par') {
                return "I suspect you have forgotten a `}', causing me to read past where you wanted me to stop.";
            }
            return "";
        }
});
