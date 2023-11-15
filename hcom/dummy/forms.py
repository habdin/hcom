#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django import forms
from .models import Dummy


class DummyForm(forms.ModelForm):
    class Meta:
        model = Dummy
        fields = ['id', 'name', 'category'] 
