// Copyright (c) 2013-2016 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

namespace SharpConfig
{
    // Enumerates the elements of a Setting that represents an array.
    internal sealed class SettingArrayEnumerator
    {
        private readonly string mStringValue;
        private readonly bool mShouldCalcElemString;
        private int mIdxInString;
        private readonly int mLastRBraceIdx;
        private int mPrevElemIdxInString;
        private int mBraceBalance;
        private bool mIsInQuotes;
        private bool mIsDone;

        public SettingArrayEnumerator(string value, bool shouldCalcElemString)
        {
            mStringValue = value;
            mIdxInString = -1;
            mLastRBraceIdx = -1;
            mShouldCalcElemString = shouldCalcElemString;
            IsValid = true;
            mIsDone = false;

            for (int i = 0; i < value.Length; ++i)
            {
                char ch = value[i];
                if (ch != ' ' && ch != '{')
                    break;

                if (ch == '{')
                {
                    mIdxInString = i + 1;
                    mBraceBalance = 1;
                    mPrevElemIdxInString = i + 1;
                    break;
                }
            }

            // Abort if no valid '{' occurred.
            if (mIdxInString < 0)
            {
                IsValid = false;
                mIsDone = true;
                return;
            }

            // See where the last valid '}' is.
            for (int i = value.Length - 1; i >= 0; --i)
            {
                char ch = value[i];
                if (ch != ' ' && ch != '}')
                    break;

                if (ch == '}')
                {
                    mLastRBraceIdx = i;
                    break;
                }
            }

            // Abort if no valid '}' occurred.
            if (mLastRBraceIdx < 0)
            {
                IsValid = false;
                mIsDone = true;
                return;
            }

            // See if this is an empty array such as "{    }" or "{}".
            // If so, this is a valid array, but with size 0.
            if (mIdxInString == mLastRBraceIdx ||
                !IsNonEmptyValue(mStringValue, mIdxInString, mLastRBraceIdx))
            {
                IsValid = true;
                mIsDone = true;
                return;
            }
        }

        public bool Next()
        {
            if (mIsDone)
                return false;

            int idx = mIdxInString;
            while (idx <= mLastRBraceIdx)
            {
                char ch = mStringValue[idx];
                if (ch == '{' && !mIsInQuotes)
                {
                    ++mBraceBalance;
                }
                else if (ch == '}' && !mIsInQuotes)
                {
                    --mBraceBalance;
                    if (idx == mLastRBraceIdx)
                    {
                        // This is the last element.
                        if (!IsNonEmptyValue(mStringValue, mPrevElemIdxInString, idx))
                        {
                            // Empty array element; invalid array.
                            IsValid = false;
                        }
                        else if (mShouldCalcElemString)
                        {
                            Current = mStringValue.Substring(
                                mPrevElemIdxInString,
                                idx - mPrevElemIdxInString
                                ).Trim();
                        }
                        mIsDone = true;
                        break;
                    }
                }
                else if (ch == '\"')
                {
                    int iNextQuoteMark = mStringValue.IndexOf('\"', idx + 1);
                    if (iNextQuoteMark > 0)
                    {
                        idx = iNextQuoteMark;
                        mIsInQuotes = false;
                    }
                    else
                        mIsInQuotes = true;
                }
                else if (ch == Configuration.ArrayElementSeparator && mBraceBalance == 1 && !mIsInQuotes)
                {
                    if (!IsNonEmptyValue(mStringValue, mPrevElemIdxInString, idx))
                    {
                        // Empty value in-between commas; this is an invalid array.
                        IsValid = false;
                    }
                    else if (mShouldCalcElemString)
                    {
                        Current = mStringValue.Substring(
                            mPrevElemIdxInString,
                            idx - mPrevElemIdxInString
                            ).Trim();
                    }

                    mPrevElemIdxInString = idx + 1;

                    // Yield.
                    ++idx;
                    break;
                }

                ++idx;
            }

            mIdxInString = idx;

            if (mIsInQuotes)
                IsValid = false;

            return IsValid;
        }

        private static bool IsNonEmptyValue(string s, int begin, int end)
        {
            for (; begin < end; ++begin)
                if (s[begin] != ' ')
                    return true;

            return false;
        }

        public string Current { get; private set; }

        public bool IsValid { get; private set; }
    }
}
