# Content Format

## Directory Structure

The directory structor of content looks like this, for example:

```yaml
2019
  - day1.md
  - day2.md
  ...
  - day23.md
  - day24.md
2020
  - day1.md
  - day2.md
  ...
  - day23.md
  - day24.md
```

Each year contains from day1<span></span>.md to day24<span></span>.md (if contains christmas, also day25<span></span>.md).

Each year contains days content of corresponding year and each day contains content of corresponding day. Format of day contents is markdown (`*.md`). For example, `2019/day24.md` contains content of day 24, year 2019.

## Inside *.md file

```markdown
# Title

Content
```

First heading of the content is the title. On the summary, markdown syntax is not applied and has maximum length of 80.
