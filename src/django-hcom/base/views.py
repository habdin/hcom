#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.shortcuts import render
from django.conf import settings
# Create your views here.


def index(request):
    """
    Returns the basic home page of the HCOM web app.
    """
    context = {"debug": settings.DEBUG}
    return render(request, "base/index.html", context)
