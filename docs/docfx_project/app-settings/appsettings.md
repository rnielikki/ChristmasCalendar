# appsettings.json

`appsettings.json` contains some site settings.

```json
{
  "title":"My Advent Calendar",
  "startYear": 2019,
  "skipYears": [],
  "containsChristmas": true,
  "baseUri": "",
  "summaryLength":80
}
```

## title

* Type: `string`
* Default: "Advent Calendar"

The title that displayed in the app. It can be theme, site name or whatever.

## startYear

* Type: `int`
* Default: 2019

The year that you started to archive.
If you don't have any archive, just put current year.

> Note: If startYear is not valid (greater than this year), it throws an exception.

## skipYears

* Type: `int`
* Default: Empty Array

If you forgot to make advent calendar in some years, put here the years.

## containsChristmas

* Type: `bool`
* Default: `false`

Some advent calendar of cultural region contains Christams, while others not. If the value is `true`, day 25 will be shown in the calendar and archives. Otherwise, day 25 won't count.

## baseUri

* Type: `string`
* Default: ""

The absolute path where the content is. If you don't use external URI but inside `contents` directory of the app (as default), you don't need to set, or let this value empty.

## summaryLength

* Type: `int`
* Default: 80

The length of the summary.
