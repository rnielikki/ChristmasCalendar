# ChristmasCalendar Project!

... It was "Advent calendar" in English, whatever.

[![Build Status](https://dev.azure.com/LumiKwon0498/KouluAss/_apis/build/status/rnielikki.ChristmasCalendar?branchName=master)](https://dev.azure.com/LumiKwon0498/KouluAss/_build/latest?definitionId=3&branchName=master)

## What is this

This helps to make advent calendar only with markdown file adding / editing.

### So, what is Advent calendar

Advent calendar is counting days until Christmas. Nowadays it starts in December, from day 1 to 24 (or 25, depending on the advent calendar / culture). Each day is covered and opened when reaches the day - it contains something, for example, any items or contents.

You can see more information about the Advent Calendar from [here](https://en.wikipedia.org/wiki/Advent_calendar).

### So what'll be title and what'll be content?

```markdown
# Today is already second day.
I wonder if there're Santa Claus. Or is this Satan Claus?
Whatever, I am wating for the white winter.
```

Then title is: *Today is already second day.*
Content is:

```html
<p>I wonder if there're Santa Claus. Or is this Satan Claus?</p>
<p>Whatever, I am wating for the white winter.</p>
```

And... the summary is:
*I wonder if there're Santa Claus. Or is this Satan Claus?
Whatever, I am wating ...*

## Why?

It's part of the software testing project, but also for trying blazor WASM!

## It's fully client-side, so you can foresee API data. It's not safe.

If someone wants spoiler, let them see it.

Also, you can set baseUri to `appsettings.json` file and define where is server-side data.

(For example, you can use Azure Function to hide unrevealed days ;) )

## Can I see example?

OK, let's see in this year!

## Why it's 1-24, not 1-25?

Because 1-24 is default. If you want day 25, try `containsChristmas` to `true` inside `appsettings.json`.

## Can I contribute?

**Sure.** VVelcome! By adding any issues or leaving pull requests. If you want to add feature, please leave issue first.
