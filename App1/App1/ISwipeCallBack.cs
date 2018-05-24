using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1
{
    public interface ISwipeCallBack
    {
        void onLeftSwipe(View view);
        void onRightSwipe(View view);
        void onTopSwipe(View view);
        void onBottomSwipe(View view);
        void onNothingSwiped(View view);
    }
}
