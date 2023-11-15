#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.shortcuts import render

# Create your views here.


def index(request):
    """
    Returns the basic home page of the HCOM web app.
    """
    return render(request, 'base/index.html')
