# appsettings.json
It's very simple. it looks like:
```json
{
  "startYear": 2019,
  "skipYears": [],
  "title":"My Advent Calendar"
}
```
## title
The title that displayed in the app. It can be the related site name, theme or whatever.

## startYear
The year that you started to archive.
If you don't have any archive, just put current year.

> Note: If startYear is not valid (greater than this year), it throws an exception.

## skipYears
If you forgot to make advent calendar in some years, put here the years.