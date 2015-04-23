
var countDown;
var SD;

function initQuestion() {

    var totalSeconds = document.getElementById("question-timer").getAttribute("data-TimeLimit");
    var timePassed = document.getElementById("question-timer").getAttribute("data-timePassed")-1;
    var totalTestSeconds = document.getElementById("test-timer").getAttribute("data-testTimeLimit");
    var testTimePassed = document.getElementById("test-timer").getAttribute("data-testTimePassed")-1;

    countDown = function () {
        if (totalTestSeconds != 0) {
            testTimePassed++;
            var diffTest = totalTestSeconds - testTimePassed;

            if (diffTest <= 0) {
                Test.goNextWithAnswer();
                return;
            }

            var testTime = getTimeFromSeconds(diffTest);
            $('#test-timer').html(testTime);
        }

        if (totalSeconds != 0) {
            timePassed++;
            var diff = totalSeconds - timePassed;

            if (diff <= 0) {
                Test.goNextWithAnswer();
                return;
            }

            var time = getTimeFromSeconds(diff);
            $('#question-timer').html(time);
        }

        SD = window.setTimeout("countDown();", 1000);
    };
    countDown();

}

function getTimeFromSeconds(diffTest) {
    var testHour = parseInt(diffTest / 3600);
    var testMin = parseInt((diffTest - 3600 * testHour) / 60);
    var testSec = parseInt(diffTest - 3600 * testHour - 60 * testMin);
    return (testHour <= 0 ? "" : testHour <= 9 ? "0" + testHour + ":" : testHour + ":") + ((testMin <= 0 && testHour <= 0) ? "" : testMin <= 9 ? "0" + testMin + ":" : testMin + ":") + (testSec <= 9 ? "0" + testSec : testSec);
}