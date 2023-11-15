#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django import forms

from .models import User


class UserLoginForm(forms.ModelForm):

    class Meta:
        model = User
        fields = ['username', 'password']
