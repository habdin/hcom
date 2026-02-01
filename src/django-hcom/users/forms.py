# vim:foldmethod=indent:ts=4
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django import forms
from django.core.exceptions import ValidationError

from .models import User


class UserCreateForm(forms.ModelForm):
    password1 = forms.CharField(
        label="Password", widget=forms.PasswordInput, min_length=8, max_length=150
    )
    password2 = forms.CharField(
        label="Confirm Password",
        widget=forms.PasswordInput,
        min_length=8,
        max_length=150,
    )

    class Meta:
        model = User
        fields = [
            "username",
            "first_name",
            "last_name",
            "email",
            "is_staff",
            "is_active",
            "city",
            "id_number",
            "gender",
            "marital_status",
        ]

    def clean_password2(self):
        """Check that password is entered correctly in both input boxes."""
        password1 = self.cleaned_data.get("password1")
        password2 = self.cleaned_data.get("password2")
        if password1 and password2 and password1 != password2:
            raise ValidationError("Passwords don't match.")
        return password2

    def save(self, commit=True):
        user = super().save(commit=False)
        user.set_password(self.cleaned_data["password1"])
        if commit:
            user.save()
        return user


class UserChangeForm(forms.ModelForm):
    password = forms.CharField(widget=forms.PasswordInput)

    class Meta:
        model = User
        fields = [
            "username",
            "first_name",
            "last_name",
            "email",
            "is_staff",
            "is_active",
            "city",
            "id_number",
            "gender",
            "marital_status",
        ]


class UserLoginForm(forms.ModelForm):
    class Meta:
        model = User
        fields = ["username", "password"]
