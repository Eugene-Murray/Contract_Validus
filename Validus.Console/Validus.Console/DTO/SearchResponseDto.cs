using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client.Search.Query;

namespace Validus.Console.DTO
{
    public class SearchResponseDto
    {
        public class SearchResponsePagingDto
        {
            public bool Active { get; set; }
            public int Value { get; set; }
            public bool Display { get; set; }

            public SearchResponsePagingDto()
            {
                Active = false;
                Display = true;
            }
        }

        private readonly List<SearchResultsBaseDto> _searchResults;

        public int TotalAvailable { get; set; }
        public int StartRow { get; set; }
        public int RowLimit { get; set; }
        public int EndRow { get; set; }
        public bool PreviousEnabled { get; set; }
        public bool NextEnabled { get; set; }
        public string[] Properties { get; set; }
        private int CurrentPosition { get; set; }
        private int MaximumPosition { get; set; }

        public List<SearchContentSourceDto> ContentSources { get; set; }

        public ResultTable Refiners { get; set; }

        public SearchResponsePagingDto FirstPagingPosition { get; private set; }

        public SearchResponsePagingDto SecondPagingPosition { get; private set; }

        public SearchResponsePagingDto ThirdPagingPosition { get; private set; }

        public SearchResponsePagingDto ForthPagingPosition { get; private set; }

        public SearchResponsePagingDto FifthPagingPosition { get; private set; }

        public bool HasPaging { get; private set; }

        public bool IsSourceRestricted
        {
            get { return ContentSources.Any(item => !item.IsSearched); }
        }

        public bool HasResults 
        {
            get { return (SearchResults.Any()); }
        }

        public SearchResponseDto(int startRow, int rowLimit, int totalAvailable)
        {
            _searchResults = new List<SearchResultsBaseDto>();
            TotalAvailable = totalAvailable;
            StartRow = startRow;
            RowLimit = rowLimit;
            ContentSources = new List<SearchContentSourceDto>();
            SetupPaging();
        }

        private void SetupPaging()
        {
            // calculate the end row
            if ((StartRow + (RowLimit - 1)) >= TotalAvailable)
                EndRow = TotalAvailable;
            else
                EndRow = (StartRow + (RowLimit - 1));
            // calculate the current position
            CurrentPosition = (StartRow + (RowLimit - 1)) / RowLimit;
            // calculate the maximum position
            double div = TotalAvailable / RowLimit;
            if ((TotalAvailable % RowLimit) == 0)
            {
                MaximumPosition = (int)Math.Floor(div);
            }
            else if ((TotalAvailable % RowLimit) > 0)
            {
                MaximumPosition = (int)Math.Floor(div) + 1;
            }
            // do we need paging
            if (MaximumPosition == 1)
            {
                HasPaging = false;
                FirstPagingPosition = new SearchResponsePagingDto {Display = false};
                SecondPagingPosition = new SearchResponsePagingDto {Display = false};
                ThirdPagingPosition = new SearchResponsePagingDto {Display = false};
                ForthPagingPosition = new SearchResponsePagingDto {Display = false};
                FifthPagingPosition = new SearchResponsePagingDto {Display = false};
            }
            else
            {
                HasPaging = true;
                // set prev and next
                PreviousEnabled = CurrentPosition != 1;
                NextEnabled = CurrentPosition < MaximumPosition;
                // Do the paging position one culculations
                FirstPagingPosition = new SearchResponsePagingDto();
                if (CurrentPosition < 4)
                {
                    FirstPagingPosition.Value = 1;
                    FirstPagingPosition.Display = true;
                    if (CurrentPosition == 1)
                    {
                        FirstPagingPosition.Active = true;
                    }
                    if (CurrentPosition > 1)
                    {
                        FirstPagingPosition.Active = false;
                    }
                }
                else
                {
                    FirstPagingPosition.Display = true;
                    FirstPagingPosition.Active = false;
                    if (CurrentPosition > (MaximumPosition - 2))
                    {
                        FirstPagingPosition.Value = MaximumPosition - 4;
                    }
                    if (CurrentPosition <= (MaximumPosition - 2))
                    {
                        FirstPagingPosition.Value = CurrentPosition - 2;
                    }
                }
                // Do the paging position two culculations
                SecondPagingPosition = new SearchResponsePagingDto();
                if (CurrentPosition < 4)
                {
                    SecondPagingPosition.Value = 2;
                    SecondPagingPosition.Display = true;
                    if ((CurrentPosition == 2) && (MaximumPosition >= 2))
                    {
                        SecondPagingPosition.Active = true;
                    }
                    if ((CurrentPosition != 2) && (MaximumPosition >= 2))
                    {
                        SecondPagingPosition.Active = false;
                    }
                }
                else
                {
                    SecondPagingPosition.Display = true;
                    SecondPagingPosition.Active = false;
                    if (CurrentPosition > (MaximumPosition - 2))
                    {
                        SecondPagingPosition.Value = MaximumPosition - 3;
                    }
                    if (CurrentPosition <= (MaximumPosition - 2))
                    {
                        SecondPagingPosition.Value = CurrentPosition - 1;
                    }
                }
                // Do the paging position two culculations
                ThirdPagingPosition = new SearchResponsePagingDto();
                if ((CurrentPosition < 4) && (MaximumPosition >= 3))
                {
                    ThirdPagingPosition.Value = 3;
                    ThirdPagingPosition.Display = true;
                    if ((CurrentPosition == 3) && (MaximumPosition >= 3))
                    {
                        ThirdPagingPosition.Active = true;
                    }
                    if ((CurrentPosition != 3) && (MaximumPosition >= 3))
                    {
                        ThirdPagingPosition.Active = false;
                    }
                }
                else if ((CurrentPosition < 4) && (MaximumPosition < 3))
                {
                    ThirdPagingPosition.Display = false;
                    ThirdPagingPosition.Active = false;
                }
                else
                {
                    ThirdPagingPosition.Display = true;
                    ThirdPagingPosition.Active = false;
                    if (CurrentPosition > (MaximumPosition - 2))
                    {
                        ThirdPagingPosition.Value = MaximumPosition - 2;
                    }
                    if (CurrentPosition <= (MaximumPosition - 2))
                    {
                        ThirdPagingPosition.Value = CurrentPosition;
                        ThirdPagingPosition.Active = true;
                    }
                }
                // Do the paging position four culculations
                ForthPagingPosition = new SearchResponsePagingDto();
                if ((CurrentPosition < 4) && (MaximumPosition >= 4))
                {
                    ForthPagingPosition.Value = 4;
                    ForthPagingPosition.Display = true;
                }
                else if ((CurrentPosition < 4) && (MaximumPosition < 4))
                {
                    ForthPagingPosition.Display = false;
                    ForthPagingPosition.Active = false;
                }
                else
                {
                    ForthPagingPosition.Display = true;
                    ForthPagingPosition.Active = false;
                    if (CurrentPosition == (MaximumPosition - 1))
                    {
                        ForthPagingPosition.Value = MaximumPosition - 1;
                        ForthPagingPosition.Active = true;
                    }
                    else if (CurrentPosition > (MaximumPosition - 1))
                    {
                        ForthPagingPosition.Value = MaximumPosition - 1;
                    }
                    else
                    {
                        ForthPagingPosition.Value = CurrentPosition + 1;
                    }
                }
                // Do the paging position five culculations
                FifthPagingPosition = new SearchResponsePagingDto();
                if ((CurrentPosition < 4) && (MaximumPosition >= 5))
                {
                    FifthPagingPosition.Value = 5;
                    FifthPagingPosition.Display = true;
                }
                else if ((CurrentPosition < 4) && (MaximumPosition < 5))
                {
                    FifthPagingPosition.Display = false;
                    FifthPagingPosition.Active = false;
                }
                else
                {
                    FifthPagingPosition.Display = true;
                    FifthPagingPosition.Active = false;
                    if (CurrentPosition == MaximumPosition)
                    {
                        FifthPagingPosition.Value = MaximumPosition;
                        FifthPagingPosition.Active = true;
                    }
                    else if (CurrentPosition == (MaximumPosition - 1))
                    {
                        FifthPagingPosition.Value = MaximumPosition;
                    }
                    else
                    {
                        FifthPagingPosition.Value = CurrentPosition + 2;
                    }
                }
            }
        }


        //public SearchResponseDto(SearchResponsePagingDto firstPagingPosition, SearchResponsePagingDto secondPagingPosition, SearchResponsePagingDto thirdPagingPosition, SearchResponsePagingDto fourthPagingPosition, SearchResponsePagingDto fifthPagingPosition)
        //{
        //    FirstPagingPosition = firstPagingPosition;
        //    SecondPagingPosition = secondPagingPosition;
        //    ThirdPagingPosition = thirdPagingPosition;
        //    FourthPagingPosition = fourthPagingPosition;
        //    FifthPagingPosition = fifthPagingPosition;
        //    _searchResults = new List<SearchResultsBaseDto>();
        //}

        /*
        public int FirstPagingPosition
        {
            get
            {
                if (CurrentPosition < 4)
                {
                    if (CurrentPosition == 1)
                    {
                        FirstPagingPostionActive = true;
                        return 1;
                    }
                    if (CurrentPosition > 1) return 1;
                }
                else
                {
                    if (CurrentPosition > (MaximumPosition - 2)) return MaximumPosition - 4;
                    if (CurrentPosition <= (MaximumPosition - 2)) return CurrentPosition - 2;
                }
                return 1;
            }
        }

        public int SecondPagingPosition
        {
            get
            {
                if (CurrentPosition < 4)
                {
                    if ((CurrentPosition == 2) &&(MaximumPosition = true))
                    {
                        SecondPagingPostionActive = true;
                        return 2;
                    }
                    if (CurrentPosition != 2) return 1;
                }
                else
                {
                    if (CurrentPosition > (MaximumPosition - 2)) return MaximumPosition - 4;
                    if (CurrentPosition <= (MaximumPosition - 2)) return CurrentPosition - 2;
                }
                return 2;
            }


        }
        */
        public List<SearchResultsBaseDto> SearchResults { get { return _searchResults;  }}

    }
}