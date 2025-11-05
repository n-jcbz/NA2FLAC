# Things to do

## A collection of notes and known problems

- Fix BRSTM size estimation, currently guessing 214,78 MB @ 55 files -> ~966,49 MB (316 MB is the actual outcome) with a factor of 4.5, so it's too high for BRSTM files (Works for BFSTM since it was properly tested but it was in the same line as BRSTM/BCSTM during testing so both BRSTM and BCSTM currently use a multiplication factor of 4.5)
    - will try ~1.5 since 214,78 * 1,47 = ~315,72