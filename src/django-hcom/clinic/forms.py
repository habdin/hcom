# vim:foldmethod=indent
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django import forms

from .models import Clinic, Physician


class ClinicForm(forms.ModelForm):
    class Meta:
        model = Clinic
        fields = ["opening_time", "closing_time", "physician", "is_archived"]


class PhysicianForm(forms.ModelForm):
    class Meta:
        model = Physician
        fields = ["first_name", "last_name", "speciality", "image"]
